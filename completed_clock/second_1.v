module second_1(clk_in, rst,clk_out,seg_data);

  input clk_in, rst;
  output clk_out;
  output [7:0] seg_data;

  wire	 [3:0]	cnt_bcd;

  bcd_counter U_bcd_cnt_1 (clk_in, rst, cnt_bcd);
  bcd_to_seg_mod U_bcd_to_seg_mod_1 (cnt_bcd, seg_data);

  freq_div_N #(10) U_freq_div_1 (.clk_in(clk_in), 
											 .rst(rst), 
											 .clk_div_N(clk_out));
											 
endmodule
