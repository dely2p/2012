module hour_1(clk, rst, seg_data);
input clk, rst;
output [7:0] seg_data;

wire [3:0] cnt_12;

mod_12_counter U_12_cnt (clk, rst, cnt_12);
seg_12_mod U_seg_12(cnt_12, seg_data);


endmodule
