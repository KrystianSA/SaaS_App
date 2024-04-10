export const useFormValidationRules = () => {
	return {
		ruleRequired: (v) => !!v || "Required",
		ruleEmail: (value) => {
			const pattern =
				/^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
			return pattern.test(value) || "Invalid adress email";
		},
		rulePassword: (value) => {
			const pattern =
				/^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,}$/;
			return pattern.test(value) || "Password must contain one digit from 1 to 9, one lowercase letter, one uppercase letter, one special character, no space, and it must be minimum 8 characters long";
		}
	};
};