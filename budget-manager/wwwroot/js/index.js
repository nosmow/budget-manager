(() => {
    'use strict'

    const getStoredTheme = () => localStorage.getItem('theme')
    const setStoredTheme = theme => localStorage.setItem('theme', theme)


    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme()
        if (storedTheme) {
            return storedTheme
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    const setTheme = theme => {
        if (theme === 'auto') {
            document.documentElement.setAttribute('data-bs-theme', (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'))
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme)
            changeTheme(theme)
        }
    }

    setTheme(getPreferredTheme())

    const showActiveTheme = (theme, focus = false) => {
        const themeSwitcher = document.querySelector('#bd-theme')

        if (!themeSwitcher) {
            return
        }

        const themeSwitcherText = document.querySelector('#bd-theme-text')
        const activeThemeIcon = document.querySelector('.theme-icon-active use')
        const btnToActive = document.querySelector(`[data-bs-theme-value="${theme}"]`)
        const svgOfActiveBtn = btnToActive.querySelector('svg use').getAttribute('href')

        document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
            element.classList.remove('active')
            element.setAttribute('aria-pressed', 'false')
        })

        btnToActive.classList.add('active')
        btnToActive.setAttribute('aria-pressed', 'true')
        activeThemeIcon.setAttribute('href', svgOfActiveBtn)
        const themeSwitcherLabel = `${themeSwitcherText.textContent} (${btnToActive.dataset.bsThemeValue})`
        themeSwitcher.setAttribute('aria-label', themeSwitcherLabel)

        if (focus) {
            themeSwitcher.focus()
        }
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    })

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme())

        document.querySelectorAll('[data-bs-theme-value]')
            .forEach(toggle => {
             
                toggle.addEventListener('click', () => {
                    const theme = toggle.getAttribute('data-bs-theme-value')
                    setStoredTheme(theme)
                    setTheme(theme)
                    showActiveTheme(theme, true)                
                })
            })
    })
})()

function Hello() {
    console.log("Hello World")
}

function changeTheme(theme) { 
    var newStyle = document.createElement('style')

    if (theme === "dark") {
        console.log("Tema oscuro")
        newStyle.innerHTML = `
            .sidebar {
                background-color: #171717;
                color: #FFF;
            }

            .sidebar li a {
                 color: #FFF;
            }

            .bg_accordion {
                 background-color: #323539;
                 color: #FFF;
             }

            .bg_accordion:enabled {
                 background-color: #323539;
                 color: #FFF;
             }


            .text_color {
                color: #FFF;
            }
        `

    } else if (theme === 'light') {
        console.log("Tema claro")
        newStyle.innerHTML = `
            .sidebar {
                background-color: #F4F4F4;
                color: #000;
            }

            .sidebar li a {
                 color: #000;
            }

            .bg_accordion {
                 background-color: #F4F4F4;
                 color: #000;
             }

            .bg_accordion:enabled {
                 background-color: #F4F4F4;
                 color: #000;
             }


            .text_color {
                color: #000;
            }

            .
        `

    }
    document.head.appendChild(newStyle)
}
