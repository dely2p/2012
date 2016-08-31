module hour_10(clk, rst, clk_ap, seg_data);
input clk, rst;
output [7:0] seg_data;
output clk_ap;

wire cnt_bcd;

mod_1_cnt_hour U_bcd_cnt_12 (clk, rst, cnt_bcd);

bcd_to_seg_mod U_bcd_to_seg_1 (cnt_bcd, seg_data);

  freq_div_N #(12) U_freq_div_1 (clk, rst, clk_ap);
endmodule
