#version 330 core
out vec4 FragColor;

struct Material{
	sampler2D diffuseTexture;
	sampler2D specularTexture;
	float shininess;
};
  
struct Light{
	vec3 position;
	vec3 direction;
	float cutoff;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;

	float constant;
    float linear;
    float quadratic;
};

uniform Material material;
uniform Light light;
uniform vec3 camPos;

in vec3 normal;
in vec3 modelPos;
in vec2 Texcoord;

void main()
{
	vec3 lightDir = normalize(light.position - modelPos);
	float theta = dot(lightDir, normalize(-light.direction));

	if (theta > light.cutoff)
	{
		// ambient color
		vec3 ambientColor = light.ambient * vec3(texture(material.diffuseTexture, Texcoord));

		// diffuse color 
		vec3 norm = normalize(normal);
		float diffusePower = max(dot(norm, lightDir), 0);
		vec3 diffuseColor = diffusePower * light.diffuse * vec3(texture(material.diffuseTexture, Texcoord));

		// specular
		vec3 viewDir = normalize(camPos - modelPos);
		vec3 reflectDir = reflect(-lightDir, normal);
		float specPow = pow(max(dot(reflectDir, viewDir), 0.0), material.shininess);
		vec3 specularColor = specPow * vec3(texture(material.specularTexture, Texcoord)) * light.specular;

		float distance = length(light.position - modelPos);
		float attenuation = 1.0 / (light.constant + light.linear * distance
						+ light.quadratic * (distance * distance));

		FragColor = vec4((ambientColor  + diffuseColor * attenuation + specularColor * attenuation), 1.0);	
	}
	else
	{
		FragColor = vec4(light.ambient  * vec3(texture(material.diffuseTexture, Texcoord)), 1.0);
	}
}

