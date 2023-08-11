import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'boldText'
})
export class BoldTextPipe implements PipeTransform {
  transform(text: string | null, keyword: string | null): string {
    if (!text || !keyword) {
      return '';
    }

    const textStr = text.toString(); // Convert non-string values to strings
    const startIndex = textStr.toUpperCase().indexOf(keyword.toUpperCase());
    if (startIndex !== -1) {
      const beforeKeyword = textStr.slice(0, startIndex);
      const keywordText = textStr.slice(startIndex, startIndex + keyword.length);
      const afterKeyword = textStr.slice(startIndex + keyword.length);
      return `${beforeKeyword}<strong>${keywordText}</strong>${afterKeyword}`;
    }

    return textStr;
  }
}
