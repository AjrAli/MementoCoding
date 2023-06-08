export class PageDetailsDto {
    totalItems: number = 0;
    skip: number;
    take: number;
    constructor() {
        this.totalItems = 0;
        this.skip = 0;
        this.take = 10;
    }
}