module dot_digit(clk, rst, seg_data_1, seg_data_2);
input clk, rst;
output [7:0] seg_data_1, seg_data_2;
wire cnt, cnt1, clk_d;

freq_div_N #(500) U_freq_div_1 (clk, rst, clk_d);
mod_1_counter U_dot_cnt (clk_d, rst, cnt);
mod_1_counter U_dot_cnt_h (clk_d, rst, cnt_1);
seg_dot U_seg_dot (cnt, seg_data_1);
seg_dot U_seg_dot2 (cnt, seg_data_2);

endmodule
