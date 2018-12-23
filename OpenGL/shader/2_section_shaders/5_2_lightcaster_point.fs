#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D diffuseTexture;
	sampler2D specularTexture;
	float shininess;
};
  
struct Light{
	vec3 position;
	float constant;
	float linear;
	float quadratic;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

uniform Material material;
uniform Light light;

uniform vec3 lightColor;
uniform vec3 camPos;

in vec3 normal;
in vec3 modelPos;
in vec2 Texcoord;

void main()
{
	float distance = length(light.position - modelPos);
	float attenuation = 1.0 / (light.constant + light.linear * distance
						+ light.quadratic * (distance * distance));
	// ambient color
	vec3 ambientColor = attenuation * light.ambient * vec3(texture(material.diffuseTexture, Texcoord));

	// diffuse color 
	vec3 norm = normalize(normal);
	//vec3 lightDir = normalize(light.position - modelPos);
	vec3 lightDir = normalize(light.position - modelPos);
	float diffusePower = max(dot(norm, lightDir), 0);
	vec3 diffuseColor = attenuation * diffusePower * light.diffuse * vec3(texture(material.diffuseTexture, Texcoord));

	// specular
	vec3 viewDir = normalize(camPos - modelPos);
	vec3 reflectDir = reflect(-lightDir, normal);
	float specPow = pow(max(dot(reflectDir, viewDir), 0.0), material.shininess);
	vec3 specularColor = attenuation * specPow * vec3(texture(material.specularTexture, Texcoord)) * light.specular;


    FragColor = vec4((ambientColor  + diffuseColor + specularColor), 1.0);
}

