module mod_1_negcnt (clk, rst, cnt);
  input     		clk, rst;
  output	cnt;
  reg	cnt;

	always @(negedge clk) begin
		if (rst) cnt <= 0;
		else
		cnt <= ~cnt;
	end

endmodule
