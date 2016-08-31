module seg_12_mod(bcd_in, seg_data);

input [3:0] bcd_in;
output [7:0] seg_data;

reg [7:0] seg_data;

always @ (bcd_in) begin
		case (bcd_in)
				4'b0000 : seg_data = 8'b0011_1111;
				4'b0001 : seg_data = 8'b0000_0110;
				4'b0010 : seg_data = 8'b0101_1011;
				4'b0011 : seg_data = 8'b0100_1111;
				4'b0100 : seg_data = 8'b0110_0110;
				4'b0101 : seg_data = 8'b0110_1101;
				4'b0110 : seg_data = 8'b0111_1101;
				4'b0111 : seg_data = 8'b0000_0111;
				4'b1000 : seg_data = 8'b0111_1111;
				4'b1001 : seg_data = 8'b0110_1111;
				4'b1010 : seg_data = 8'b0011_1111;
				4'b1011 : seg_data = 8'b0000_0110;
				4'b1100 : seg_data = 8'b0101_1011;
				default : seg_data = 8'b0000_0000;
	  endcase
end

endmodule
