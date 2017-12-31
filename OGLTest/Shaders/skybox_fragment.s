#version 450 core

in vec3 frag_tex_coord;

uniform samplerCube skybox;

out vec4 color;

void main()
{
	color = texture(skybox, frag_tex_coord);
	// color = vec4(1.0, 1.0, 1.0, 1.0);
}