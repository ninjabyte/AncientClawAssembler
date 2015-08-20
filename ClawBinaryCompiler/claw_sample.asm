;; Claw Demo
;; Calculate the sum of two numbers
;; If they are 200, write them to stack B

	LET16 B
.dbs "Hello World"
.db16u (101)
	LET16 A
.db16u (99)
	ADD8 A
	LET16 A
.db16u (200)
	SUB16 A
	LET16 A
.lbl success
	BRZ A
	END
success:
	MOV8 A B
