#version 330 core
out float FragColor;

in vec2 TexCoords;

uniform sampler2D gPosition;
uniform sampler2D gNormal;
uniform sampler2D gNoise;

uniform vec3 samples[64];
uniform mat4 projection;

uniform float kernelSize;
uniform float radius;
uniform float power;
const vec2 noiseScale = vec2(1280/4.0, 720/4.0);
const float bias = 0.025;

void main()
{           	
	vec3 fragPos = texture(gPosition, TexCoords).xyz;
	vec3 normal = texture(gNormal, TexCoords).rgb;
	vec3 randomVec = texture(gNoise, TexCoords * noiseScale).xyz;

	vec3 tangent = normalize(randomVec - normal * dot(randomVec, normal));
	vec3 bitangent = cross(normal, tangent);
	mat3 TBN = mat3(tangent, bitangent, normal);

	float occlusion = 0.0;
	for(int i = 0; i < kernelSize; i ++)
	{
		vec3 sample = TBN * samples[i];
		sample = fragPos + sample * radius;

		vec4 offset = vec4(sample, 1.0);
		offset = projection * offset;
		offset.xyz /= offset.w;
		offset.xyz = offset.xyz * 0.5 + 0.5;

		float depthValue = texture(gPosition, offset.xy).z;
		float rangeCheck = smoothstep(0.0, 1.0, radius / abs(fragPos.z - depthValue));
		occlusion += ((depthValue >= sample.z + bias) ? 1.0 : 0.0) * rangeCheck;
	}
	occlusion = 1.0 - (occlusion / kernelSize);
	occlusion = pow(occlusion, power);
	FragColor = occlusion;
}
