module ampm_digit(clk, rst, seg_data_ap);

  input		      clk, rst;
  output [7:0]	seg_data_ap;

  wire cnt_ap;
  mod_1_counter U_ap_cnt (clk, rst, cnt_ap);
  ap_seg_mod U_ap_seg (cnt_ap, seg_data_ap);

endmodule
