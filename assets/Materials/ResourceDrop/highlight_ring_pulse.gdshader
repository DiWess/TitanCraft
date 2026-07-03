shader_type spatial;
render_mode unshaded, blend_add, cull_disabled, depth_draw_never;

uniform vec4 ring_color : source_color = vec4(1.0, 0.72, 0.08, 0.72);
uniform float pulse_speed = 2.4;
uniform float pulse_strength = 0.28;

void fragment() {
    float pulse = 0.72 + sin(TIME * pulse_speed) * pulse_strength;
    ALBEDO = ring_color.rgb;
    EMISSION = ring_color.rgb * pulse;
    ALPHA = ring_color.a * pulse;
}
