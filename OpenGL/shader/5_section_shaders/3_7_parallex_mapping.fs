#version 330 core
out vec4 FragColor;

uniform sampler2D diffuseMap;
uniform sampler2D normalMap;
uniform sampler2D depthMap;

in VS_OUT {
      vec3 FragPos;
      vec3 Normal;
      vec2 TexCoords;
      vec3 TangentLightPos;
      vec3 TangentViewPos;
      vec3 TangentFragPos;
} fs_in;

uniform float height_scale;

vec2 ParallexMapp(vec2 texCoords, vec3 viewDir);

void main()
{    
    vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentFragPos);
	vec2 texCoords = ParallexMapp(fs_in.TexCoords, viewDir);
	if(texCoords.x > 1.0 || texCoords.y > 1.0 || texCoords.x < 0.0 || texCoords.y < 0.0)
		discard;

	vec3 textureNormal = texture(normalMap, texCoords).rgb;
	textureNormal = normalize(textureNormal * 2.0 - 1.0);

	// Get diffuse color
    vec3 color = texture(diffuseMap, texCoords).rgb;
    // Ambient
    vec3 ambient = 0.1 * color;
    // Diffuse
    vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);
    float diff = max(dot(lightDir, textureNormal), 0.0);
    vec3 diffuse = diff * color;
    // Specular
    vec3 reflectDir = reflect(-lightDir, textureNormal);
    vec3 halfwayDir = normalize(lightDir + viewDir);  
    float spec = pow(max(dot(textureNormal, halfwayDir), 0.0), 64.0);
  
    vec3 specular = vec3(0.2) * spec;
    FragColor = vec4(ambient + diffuse + specular, 1.0f);
}

vec2 ParallexMapp(vec2 texCoords, vec3 viewDir)
{
	float height =  texture(depthMap, texCoords).r;    
    vec2 p = viewDir.xy / viewDir.z * (height * height_scale);
    return texCoords - p;    
}

