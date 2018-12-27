#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 ModelPos;

uniform vec3 camPos;
uniform samplerCube skybox;

void main()
{        
	float ratio = 1.0 / 1.52;
	vec3 viewDir = normalize(ModelPos - camPos);
	vec3 R = refract(viewDir, normalize(Normal), ratio);	
	FragColor = texture(skybox, R);
}
