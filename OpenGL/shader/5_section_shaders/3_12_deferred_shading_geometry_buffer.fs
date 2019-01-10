#version 330 core
layout(location = 0)out vec3 gPosition;
layout(location = 1)out vec3 gNormal;
layout(location = 2)out vec4 gAlbedoSpec;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;


uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;

void main()
{           	
	gPosition = FragPos; // position buffer
	gNormal = normalize(Normal); //normal buffer 
	gAlbedoSpec.rgb = texture(texture_diffuse1, TexCoords).rgb;
	gAlbedoSpec.a = texture(texture_specular1, TexCoords).r;
}