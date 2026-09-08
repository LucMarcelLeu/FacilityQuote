import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslatePipe } from '@ngx-translate/core';

import { LanguageSwitcherComponent } from '../language-switcher/language-switcher';

import {
    LanguageService,
    Language
} from '../../../core/services/language.service';

@Component({
    selector: 'app-site-header',
    standalone: true,
    imports: [
        RouterLink,
        TranslatePipe, 
        LanguageSwitcherComponent
    ],
    templateUrl: './site-header.html',
    styleUrl: './site-header.scss'
})
export class SiteHeaderComponent {

    private readonly languageService = inject(LanguageService);

    get language(): Language {
        return this.languageService.language;
    }

    setLanguage(language: Language): void {
        this.languageService.setLanguage(language);
    }
}