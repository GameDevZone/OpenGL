#version 330 core
out vec4 FragColor;

in vec2 TexCoords;

uniform sampler2D position;
uniform sampler2D normal;
uniform sampler2D albedo;
uniform sampler2D ssao;

struct Light {
    vec3 Position;
    vec3 Color;

	float Linear;
    float Quadratic;
};
uniform Light light;

void main()
{           	
	vec3 FragPos = texture(position, TexCoords).rgb;
	vec3 Normal = texture(normal, TexCoords).rgb;
	vec3 Diffuse = texture(albedo, TexCoords).rgb;
    float ao = texture(ssao, TexCoords).r;

	vec3 lighting = Diffuse * 0.3 * ao;
	vec3 viewDir = normalize(- FragPos);

	// diffuse
    vec3 lightDir = normalize(light.Position - FragPos);
    vec3 diffuse = max(dot(Normal, lightDir), 0.0) * Diffuse * light.Color;
    // specular
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    float spec = pow(max(dot(Normal, halfwayDir), 0.0), 16.0);
    vec3 specular = light.Color * spec;
    // attenuation
    float distance = length(light.Position - FragPos);
    float attenuation = 1.0 / (1.0 + light.Linear * distance + light.Quadratic * distance * distance);
    diffuse *= attenuation;
    specular *= attenuation;
    lighting += diffuse + specular;       


	FragColor = vec4(lighting, 1.0);
}
