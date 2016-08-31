module mod_1_counter (clk, rst, cnt);
  input     		clk, rst;
  output	cnt;
  reg	cnt;

	always @(posedge clk or posedge rst) begin
		if (rst) cnt <= 0;
		else
		cnt <= ~cnt;
	end

endmodule
