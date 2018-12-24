#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D diffuseTexture;
	sampler2D specularTexture;
	float shininess;
};
  
// directional light
struct DirLight{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirLight;

// point lights
struct PointLight{
	vec3 position;
	float constant;
	float linear;
	float quadratic;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

struct SpotLight{
	vec3 position;
	vec3 direction;
	float cutoff;
	float outerCutoff;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
	float linear;
	float quadratic;
};
uniform SpotLight spotLight;


uniform Material material;
uniform vec3 camPos;

in vec3 normal;
in vec3 modelPos;
in vec2 Texcoord;

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 viewDir, vec3 fragPos);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 viewDir, vec3 fragPos);

void main()
{
	vec3 normal = normalize(normal);
	vec3 viewDir = normalize(camPos - modelPos);

	// directional light result
	vec3 dirResult = CalcDirLight(dirLight, normal, viewDir);

	// point lights results
	vec3 pointResults = vec3(0.0);
	for(int i = 0; i < NR_POINT_LIGHTS; i ++)
	{
		pointResults += CalcPointLight(pointLights[i], normal, viewDir, modelPos);
	}

	// spot light 
	vec3 spotResult = CalcSpotLight(spotLight, normal, viewDir, modelPos);

	FragColor = vec4((dirResult + pointResults + spotResult), 1.0);
}


vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
	vec3 lightDir = normalize(-light.direction);

	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	// specular
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

	vec3 ambient = light.ambient * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 diffuse = light.diffuse * diff * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 specular = light.specular * spec * texture(material.specularTexture, Texcoord).rgb;

	return (ambient + diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 viewDir, vec3 fragPos)
{
	vec3 lightDir = normalize(light.position - fragPos);
	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	// specular
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	
	// attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 /(light.constant + light.linear * distance + 
						light.quadratic * (distance * distance));
	
	vec3 ambient = light.ambient * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 diffuse = light.diffuse * diff * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 specular = light.specular * spec * texture(material.specularTexture, Texcoord).rgb;

	ambient *= attenuation;
	diffuse *= attenuation;
	specular *= attenuation;

	return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 viewDir, vec3 fragPos)
{
	vec3 lightDir = normalize(light.position - fragPos);

	// diffuse
	float diff = max(dot(normal, lightDir), 0.0);
	// specular
	vec3 reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	
	// attenuation
	float distance = length(light.position - fragPos);
	float attenuation = 1.0 /(light.constant + light.linear * distance + 
						light.quadratic * (distance * distance));

	// spotlight intensity
	float theta = dot(lightDir, normalize(-light.direction));
	float epsilon = light.cutoff - light.outerCutoff;
	float intensity = clamp((theta - light.outerCutoff) / epsilon, 0.0, 1.0);

	vec3 ambient = light.ambient * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 diffuse = light.diffuse * diff * texture(material.diffuseTexture, Texcoord).rgb;
	vec3 specular = light.specular * spec * texture(material.specularTexture, Texcoord).rgb;

	ambient *= attenuation * intensity;
	diffuse *= attenuation * intensity;
	specular *= attenuation * intensity;

	return (ambient + diffuse + specular);
}