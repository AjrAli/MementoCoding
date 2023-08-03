import { TypeDto } from "../utilities/type-dto";

export class AccountDto extends TypeDto {
    username: string;
    password: string;
    firstName: string;
    lastName: string;
    email: string;
    emailConfirmed: boolean;
    constructor() {
        super('AccountDto');
        this.username = '';
        this.password = '';
        this.firstName = '';
        this.lastName = '';
        this.email = '';
        this.emailConfirmed = true;
    }
}