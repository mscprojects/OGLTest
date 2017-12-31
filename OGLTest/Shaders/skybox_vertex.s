#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
layout (location = 3) in vec2 tex_coord;

uniform mat4 view;
uniform mat4 projection;

out vec3 frag_tex_coord;

out gl_PerVertex
{
	vec4 gl_Position;
};

void main()
{
	gl_Position = projection * view  * vec4(position, 1.0);
	frag_tex_coord = position;
}