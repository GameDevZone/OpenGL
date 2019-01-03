#version 330 core
out vec4 FragColor;

uniform sampler2D texture_diffuse1;
uniform sampler2D texture_specular1;
uniform sampler2D texture_normal1;

in VS_OUT {
      vec3 FragPos;
      vec3 Normal;
      vec2 TexCoords;
      vec3 TangentLightPos;
      vec3 TangentViewPos;
      vec3 TangentFragPos;
} fs_in;

void main()
{    
	vec3 textureNormal = texture(texture_normal1, fs_in.TexCoords).rgb;
	textureNormal = normalize(textureNormal * 2.0 - 1.0);

	 // Get diffuse color
      vec3 color = texture(texture_diffuse1, fs_in.TexCoords).rgb;
      // Ambient
      vec3 ambient = 0.1 * color;
      // Diffuse
      vec3 lightDir = normalize(fs_in.TangentLightPos - fs_in.TangentFragPos);
      float diff = max(dot(lightDir, textureNormal), 0.0);
      vec3 diffuse = diff * color;
      // Specular
      vec3 viewDir = normalize(fs_in.TangentViewPos - fs_in.TangentFragPos);
      vec3 reflectDir = reflect(-lightDir, textureNormal);
      vec3 halfwayDir = normalize(lightDir + viewDir);  
      float spec = pow(max(dot(textureNormal, halfwayDir), 0.0), 64.0);
  
       vec3 specular = vec3(0.2) * spec;
      FragColor = vec4(ambient + diffuse + specular, 1.0f);
}

