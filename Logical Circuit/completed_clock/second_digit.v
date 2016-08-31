module second_digit(clk, rst, clk_m, seg_data_s_1, seg_data_s_10);

  input		      clk, rst;
  output          clk_m;
  output [7:0]	seg_data_s_1, seg_data_s_10;

  wire clk_1hz, clk_10hz;
 
  freq_div_N #(1000) U_freq_div_1000 (.clk_in(clk), 
											     .rst(rst), 
											     .clk_div_N(clk_1hz));

  second_1 U_sec_1(.clk_in(clk_1hz), 
						 .rst(rst), 
						 .clk_out(clk_10hz), 
						 .seg_data(seg_data_s_1));
  
  second_10 U_sec_10(.clk_in(clk_10hz), 
						   .rst(rst), 
					   	.clk_out(clk_m), 
						   .seg_data(seg_data_s_10));


endmodule
