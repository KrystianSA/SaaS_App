export const useWebApiResponseParser = () => {
    return {
        getErrorMessage: (response, messageMap, fieldMap) => {
            if (response) {
                if (response.status === 401) {
                    return "Brak dostępu";
                }
                else if (response.status === 422) {
                    if (response._data && response._data.errors) {
                        let messages = [];
                        const messageMapEx = {...defaultValidationMessages, ...messageMap };
                        const fieldMapEx = { ...defaultFieldNames, ...fieldMap };

                        response._data.errors.forEach(e => {
                            messages.push(`Błąd w polu ${fieldMapEx[e.fieldName] ?? e.fieldName}: ${messageMapEx[e.error] ?? e.error}`);
                        });

                        return messages.join('\n');
                    }

                    return "Wprowadzono niepoprawne dane";
                }
                else {
                    if (response._data && response._data.error) {
                        return messageMap[response._data.error] ?? response._data.error;
                    }

                    return "Wystąpił błąd";
                }

            }

            return "";
        }
    };
};

const defaultValidationMessages = {
    "NotEmptyValidator" : "Pole nie może być puste",
    "EmailValidator" : "Niepoprawny adres email"
}

const defaultFieldNames = {
    "Email" : "Email",
    "Password": "Hasło"
}