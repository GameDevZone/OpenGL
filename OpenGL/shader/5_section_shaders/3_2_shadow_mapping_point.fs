#version 330 core
out vec4 FragColor;

in VS_OUT{
	vec3 FragPos;
	vec3 Normal;
	vec2 TexCoords;
}fs_in;

uniform sampler2D diffuseTexture;
uniform samplerCube depthMap;

uniform vec3 lightPos;
uniform vec3 viewPos;

uniform float far_plane;

vec3 gridSamplingDisk[20] = vec3[]
(
   vec3(1, 1,  1), vec3( 1, -1,  1), vec3(-1, -1,  1), vec3(-1, 1,  1), 
   vec3(1, 1, -1), vec3( 1, -1, -1), vec3(-1, -1, -1), vec3(-1, 1, -1),
   vec3(1, 1,  0), vec3( 1, -1,  0), vec3(-1, -1,  0), vec3(-1, 1,  0),
   vec3(1, 0,  1), vec3(-1,  0,  1), vec3( 1,  0, -1), vec3(-1, 0, -1),
   vec3(0, 1,  1), vec3( 0, -1,  1), vec3( 0, -1, -1), vec3( 0, 1, -1)
);


float ShadowCalculation(vec3 fragPos)
{
	vec3 fragToLight = fragPos - lightPos;
	
	float closetDepth = texture(depthMap, fragToLight).r;
	closetDepth *= far_plane;
	// current depth
	float currentDepth = length(fragToLight);

	float shadow = 0.0;
	float bias   = 0.15;
	int samples  = 20;
	float viewDistance = length(viewPos - fragPos);
	float diskRadius = 0.05;
	//float diskRadius = (1.0 + (viewDistance / far_plane)) / 25.0;  
	for(int i = 0; i < samples; ++i)
	{
		float closestDepth = texture(depthMap, fragToLight + gridSamplingDisk[i] * diskRadius).r;
		closestDepth *= far_plane;   // Undo mapping [0;1]
		if(currentDepth - bias > closestDepth)
			shadow += 1.0;
	}
	shadow /= float(samples);  

		//shadow = closetDepth; // for shadow map visual

	return shadow;
}

void main()
{           
	vec3 color = texture(diffuseTexture, fs_in.TexCoords).rgb;
    vec3 normal = normalize(fs_in.Normal);
    vec3 lightColor = vec3(0.8);
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
	float shadow = ShadowCalculation(fs_in.FragPos);
	vec3 final = (ambient + (1.0 - shadow) * (diffuse + specular)) * color;

	//final = vec3(shadow / far_plane); // for shadow map visual

	FragColor = vec4(final, 1.0);
}
