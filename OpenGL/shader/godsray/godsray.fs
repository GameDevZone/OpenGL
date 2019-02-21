#version 330 core
out vec4 FragColor;

uniform sampler2D scene;

in vec4 lightPosScreenSpace;
in vec2 TexCoord;

const float Density = 0.325f;
const int NUM_SAMPLES = 100;
const float Weight = 0.25f;
const float Decay = 0.96875;

uniform bool godsray = false;

const float exposure = 1.0f;

void main()
{
	if (godsray)
	{
		vec2 lightPos = lightPosScreenSpace.xy/lightPosScreenSpace.w; // NDC space [-1, 1]
		lightPos = (lightPos * 0.5f) + 0.5f;
		vec2 deltaTexCoord = TexCoord - lightPos;
		deltaTexCoord *= 1.0f / float(NUM_SAMPLES) * Density;
		vec3 color;// = texture(scene, TexCoord).rgb;
		float illuminationDecay = 1.0f;
		vec2 orginTexCoord = TexCoord;

		for(int i = 0; i < NUM_SAMPLES; i++)
		{
			orginTexCoord -= deltaTexCoord;
			vec3 sampleColor = texture(scene, orginTexCoord).rgb;
			sampleColor *= illuminationDecay * Weight;

			color += sampleColor;

			illuminationDecay *= Decay;
		}
		color = color/ (color + vec3(1.0));
		FragColor = vec4(color * exposure, 1);
	}
	else
	{
		FragColor = texture(scene, TexCoord);
	}
}