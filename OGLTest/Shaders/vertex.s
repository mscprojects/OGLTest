#version 450 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 normal;
layout (location = 2) in vec4 color;
layout (location = 3) in vec2 tex_coord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec4 frag_color;
out vec2 frag_tex_coord;
out vec3 frag_normal;
out vec3 frag_position;

out gl_PerVertex
{
	vec4 gl_Position;
};

void main()
{
	gl_Position = projection * view * model * vec4(position, 1.0);

	frag_color = color;
	frag_tex_coord = tex_coord;
	frag_normal = normal;
	frag_position = vec3(model * vec4(position, 1.0));
}