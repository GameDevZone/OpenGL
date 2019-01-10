#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D imageBuffer;

void main()
{           	
	float ssao = texture(imageBuffer, TexCoords).r;
	FragColor = vec4(vec3(ssao), 1.0);
}
