module minute_digit(clk, rst, clk_h, clk_out, seg_data_m_1, seg_data_m_10);

  input		      clk, rst;
  output          clk_h, clk_out;
  output [7:0]	seg_data_m_1, seg_data_m_10;

  wire clk_10hz;
 
  second_1 U_min_1(.clk_in(clk), 
						 .rst(rst), 
						 .clk_out(clk_10hz), 
						 .seg_data(seg_data_m_1));
  
  second_10 U_min_10(.clk_in(clk_10hz), 
						   .rst(rst), 
					   	.clk_out(clk_h), 
						   .seg_data(seg_data_m_10));
assign clk_out = clk;

endmodule
