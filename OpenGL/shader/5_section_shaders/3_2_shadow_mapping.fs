#version 330 core
out vec4 FragColor;

in VS_OUT{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
	vec4 FragPosLightSpace;
}fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D shadowMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

float ShadowCalculation(vec4 FragPosLightSpace)
{
	// perform perspective divide
	vec3 projectCoords = FragPosLightSpace.xyz / FragPosLightSpace.w;

	// transform to [0, 1]
	projectCoords = projectCoords * 0.5 + 0.5;

	// get closeth from shadow map
	float closetDepth = texture(shadowMap, projectCoords.xy).r;

	// current depth
	float currentDepth = projectCoords.z;

	float shadow = currentDepth > closetDepth ? 1.0 : 0.0;

	return shadow;
}

void main()
{           
	vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightColor = vec3(0.3);
    // ambient
    vec3 ambient = 0.3 * color;
    // diffuse
    vec3 lightDir = normalize(lightPos - fs_in.FragPos);
    float diff = max(dot(lightDir, normal), 0.0);
    vec3 diffuse = diff * lightColor;
    // specular
    vec3 viewDir = normalize(viewPos - fs_in.FragPos);
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = 0.0;
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
    vec3 specular = spec * lightColor;    

	// shadow
	float shadow = ShadowCalculation(fs_in.FragPosLightSpace);
	vec3 final = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;

	FragColor = vec4(final, 1.0);
}
