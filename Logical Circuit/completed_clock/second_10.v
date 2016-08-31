module second_10(clk_in,rst,clk_out,seg_data);

  input clk_in, rst;
  output clk_out;
  output [7:0] seg_data;

  wire	 [2:0]	cnt_digit_10;

  mod_6_counter U_cnt_digit_10_ (clk_in, rst, cnt_digit_10);
  bcd_to_seg_mod U_bcd_to_seg_mod_10 (cnt_digit_10,seg_data);

  freq_div_N #(6) U_freq_div_10 (.clk_in(clk_in), 
											 .rst(rst), 
											 .clk_div_N(clk_out));
											 
endmodule
