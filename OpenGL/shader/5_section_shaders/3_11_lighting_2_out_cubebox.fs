#version 330 core
layout(location = 0) out vec4 FragColor;
layout(location = 1) out vec4 BrightColor;
in VS_OUT
{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}fs_in;

struct Light{
	vec3 Position;
	vec3 Color;
};
uniform Light lights[4];
uniform sampler2D diffuseTexture;

void main()
{           	
	vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
	vec3 normal = normalize(fs_in.Normal);

	vec3 lighting = vec3(0.0);
	for	(int i = 0; i < 4; i++)
	{
		vec3 lightDir = normalize(lights[i].Position - fs_in.FragPos);

		float diff = max(dot(lightDir, normal), 0.0);
		vec3 diffuse = diff * color * lights[i].Color;
		vec3 result = diffuse;

		float distance = length(fs_in.FragPos - lights[i].Position);
		result *= 1.0 / (distance * distance);

		lighting += result;
	}

	
    FragColor = vec4(lighting, 1.0);

	float brightness = dot(FragColor.rgb, vec3(0.333, 0.333, 0.333));
	if (brightness > 0.333)
		BrightColor = vec4(FragColor.rgb, 1.0);
	else
		BrightColor = vec4(0.0, 0.0, 0.0, 1.0);
}
