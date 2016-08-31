module hour_digit(clk, rst,clk_ap, seg_data_h_1, seg_data_h_10);

  input		      clk, rst;
  output          clk_ap;
  output [7:0]	seg_data_h_1, seg_data_h_10;

  
  hour_1 U_hour_1(clk, rst, seg_data_h_1);

  
  hour_10 U_hour_clk(clk, rst, clk_ap, seg_data_h_10); 

endmodule
