#version 330 core
out vec4 FragColor;

struct Material{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};
  
uniform Material material;

struct Light{
	vec3 position;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

uniform Light light;

uniform vec3 lightColor;
uniform vec3 lightPos;
uniform vec3 camPos;

in vec3 normal;
in vec3 modelPos;

void main()
{
	// ambient color
	vec3 ambientColor = light.ambient * material.ambient;

	// diffuse color 
	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(lightPos - modelPos);
	float diffusePower = max(dot(norm, lightDir), 0);
	vec3 diffuseColor = diffusePower * material.diffuse * light.diffuse;

	// specular
	vec3 viewDir = normalize(camPos - modelPos);
	vec3 reflectDir = reflect(-lightDir, normal);
	float specPow = pow(max(dot(reflectDir, viewDir), 0.0), material.shininess);
	vec3 specularColor = specPow * material.specular * light.specular;


    FragColor = vec4((ambientColor  + diffuseColor + specularColor), 1.0);
}

