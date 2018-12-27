#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 ModelPos;

uniform vec3 camPos;
uniform samplerCube skybox;

void main()
{        
	vec3 viewDir = normalize(ModelPos - camPos);
	vec3 R = reflect(viewDir, normalize(Normal));	
	FragColor = texture(skybox, R);
}
