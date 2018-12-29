#version 330 core
out vec4 FragColor;

in vec3 Normal;
in vec3 ModelPos;
in vec2 TexCoords;

uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
//uniform sampler2D texture_height1;
  
uniform vec3 camPos;
uniform samplerCube skybox;
struct DirLight{
	vec3 direction;

	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};
uniform DirLight dirLight;
vec3 CalcDirectionalLight(DirLight light, vec3 normal, vec3 viewDir, vec3 fragPos);

void main()
{        
	vec3 viewDir = normalize(camPos - ModelPos);
	vec3 normal = normalize(Normal);

	 // Diffuse
    vec4 diffuse_color = vec4(CalcDirectionalLight(dirLight, normal, viewDir, ModelPos), 1.0);
    // Reflection
    vec3 I = normalize(ModelPos - camPos);
    vec3 R = reflect(I, normalize(Normal));
    float reflect_intensity = texture(texture_specular1, TexCoords).r;
    vec4 reflect_color;
    if(reflect_intensity > 0.1)
    {
		reflect_color = texture(skybox, R) * reflect_intensity;
	}

    FragColor = diffuse_color + reflect_color;
}

vec3 CalcDirectionalLight(DirLight light, vec3 normal, vec3 viewDir, vec3 fragPos)
{
	float diff = max(dot(normalize(-light.direction), normal), 0.0);
	
	vec3 reflectDir = reflect(light.direction, normal);
	float spec = pow(max(dot(reflectDir, viewDir), 0.0), 32.0);

	vec3 ambient = light.ambient * texture(texture_diffuse1, TexCoords).rgb;
	vec3 diffuse = light.diffuse * diff * texture(texture_diffuse1, TexCoords).rgb;
	vec3 specular = light.specular * spec * texture(texture_specular1, TexCoords).rgb * 0.1;

	return (ambient + diffuse + specular);
}
