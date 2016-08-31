module mod_1_cnt_hour (clk, rst, cnt);
  input     		clk, rst;
  output	cnt;
  wire [4:0] cnt_12;
  reg cnt;

mod_12_counter U_cnt_12 (clk, rst, cnt_12);

	always @(posedge clk or posedge rst) begin
		if (rst) cnt <= 0;
		else begin
			if(cnt_12 == 9)
			cnt <= 1;
			else begin
			if(cnt_12 == 12)
			cnt <= 0;
			end
		end
	end

endmodule
