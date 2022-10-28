export interface RegistrationDto {
    "username": string,
    "firstName": string | null,
    "lastName": string | null,
    "password": string,
    "confirmPassword": string
  }