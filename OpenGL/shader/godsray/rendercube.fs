#version 330 core
layout(location = 0) out vec4 FragColor;
layout(location = 1) out vec4 BrightColor;
in VS_OUT
{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}fs_in;


uniform vec3 LightPosition;
uniform vec3 lightColor;
uniform sampler2D diffuseTexture;

void main()
{           	
	vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
	vec3 normal = normalize(fs_in.Normal);

	vec3 lighting = vec3(0.0);
	vec3 lightDir = normalize(LightPosition - fs_in.FragPos);

	float diff = max(dot(lightDir, normal), 0.0);
	vec3 diffuse = diff * color * lightColor;
	vec3 result = diffuse;

	float distance = length(fs_in.FragPos - LightPosition);
	result *= 1.0 / (distance * distance);

	lighting += result;

	
    FragColor = vec4(lighting, 1.0);


	BrightColor = vec4(0.0, 0.0, 0.0, 1.0);
}
