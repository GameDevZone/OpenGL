#version 330 core
out vec4 FragColor;
  
uniform vec3 objectColor;
uniform vec3 lightColor;

uniform vec3 lightPos;
uniform vec3 camPos;

in vec3 normal;
in vec3 modelPos;

void main()
{
	// ambient color
	float ambientStrenth = 0.1f;
	vec3 ambientColor = lightColor * ambientStrenth;

	// diffuse color 
	vec3 norm = normalize(normal);
	vec3 lightDir = normalize(lightPos - modelPos);
	float diffusePower = max(dot(norm, lightDir), 0);
	vec3 diffuseColor = diffusePower * lightColor;

	// specular
	float specularStrenth = 1f;
	vec3 viewDir = normalize(camPos - modelPos);
	vec3 reflectDir = reflect(-lightDir, normal);
	float specPow = pow(max(dot(reflectDir, viewDir), 0.0), 128);
	vec3 specular = specPow * specularStrenth * lightColor;


    FragColor = vec4((ambientColor  + diffuseColor + specular)* objectColor, 1.0);
}

