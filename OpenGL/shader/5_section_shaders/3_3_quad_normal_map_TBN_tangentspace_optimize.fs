#version 330 core
out vec4 FragColor;

in VS_OUT
{
	vec3 FragPos;
	vec2 TexCoords;
	vec3 TangentLightPos;
    vec3 TangentViewPos;
    vec3 TangentFragPos;
}fs_in;

uniform sampler2D diffuseTexture;
uniform sampler2D normalMap;

vec3 BlinnPhong(vec3 fragPos, vec3 normal, vec3 lightDir, vec3 viewDir);

void main()
{           
	// normal from texture
	vec3 normal = texture(normalMap, fs_in.TexCoords).rgb;
	normal  = normalize(normal * 2.0 - 1.0);

	// lightdir
	vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);

	// viewDir 
	vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentFragPos);

	vec3 result = BlinnPhong(fs_in.TangentFragPos, normal, lightDir, viewDir);

    FragColor = vec4(result, 1.0);
}


vec3 BlinnPhong(vec3 fragPos, vec3 normal, vec3 lightDir, vec3 viewDir)
{
	vec3 textureColor = texture(diffuseTexture, fs_in.TexCoords).rgb;

	// ambient 
	vec3 ambient = textureColor * 0.1;

	// diffuse
	float diff = max(dot(lightDir, normal), 0.0);
	vec3 diffuse = textureColor * diff;

	// specluar
	vec3 reflectDir = reflect(-lightDir, normal);
	vec3 halfwayDir = normalize(lightDir + viewDir);
	float spec = pow(max(dot(normal, halfwayDir), 0.0), 64.0);
	vec3 specular = spec * vec3(0.2);

	return ambient + diffuse + specular;
}