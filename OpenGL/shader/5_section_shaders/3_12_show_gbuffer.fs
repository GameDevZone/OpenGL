#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D imageBuffer;

void main()
{           	
	const float gamma = 2.2;
	vec3 hdrColor = texture(imageBuffer, TexCoords).rgb;

	// gamma correction 
	vec3 result = pow(hdrColor, vec3(1.0 / gamma));
	FragColor = vec4(hdrColor, 1.0);

}
