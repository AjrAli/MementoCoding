import { EntityUrl } from "src/app/enum/entity";


export class TypeDto {
    typeInstance: string;
    urlEntity!: EntityUrl;
    [key: string]: any;
    constructor(typeInstance: string, urlEntity?: EntityUrl) {
        this.typeInstance = typeInstance;
        if (urlEntity)
            this.urlEntity = urlEntity;
    }
}