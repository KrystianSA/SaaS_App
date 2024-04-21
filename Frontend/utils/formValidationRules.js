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
		},
		ruleUrl: (value) => {
			const pattern =
				/^(https?|ftps?):\/\/(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}(?::(?:0|[1-9]\d{0,3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5]))?(?:\/(?:[-a-zA-Z0-9@%_\+.~#?&=]+\/?)*)?$/;
			return pattern.test(value) || "Invalid Url";
		}
	};
};