'use strict'

document.addEventListener('DOMContentLoaded', () => {
    const currentCourtType = document.querySelector('.court-display');
    const currentModeType = document.querySelector('.mode-display');
    const courtTypes = document.querySelectorAll('.court-type a');
    const modeTypes = document.querySelectorAll('.mode-type a');
    const checkbox = document.querySelector('#showDrop');

    courtTypes.forEach(courtType => {
        courtType.addEventListener('click', function(event) {
            // If link is already selected, prevent further clicks
            if (courtType.classList.contains('selected')) {
                event.preventDefault();
                return;
            }
            // Prevent navigation
            event.preventDefault();
            currentCourtType.textContent = courtType.textContent;
            courtTypes.forEach(item => {
                item.classList.remove('selected');
                item.classList.remove('unclickable');
            });
            courtType.classList.add('selected');
            courtType.classList.add('unclickable');

            checkbox.checked = false;
        })

    })

    modeTypes.forEach(modeType => {
        modeType.addEventListener('click', function(event) {
            // If link is already selected, prevent further clicks
            if (modeType.classList.contains('selected')) {
                event.preventDefault();
                return;
            }
            // Prevent navigation
            event.preventDefault();

            currentModeType.textContent = modeType.textContent;
            modeTypes.forEach(item => {
                item.classList.remove('selected');
                item.classList.remove('unclickable');
            });
            modeType.classList.add('selected');
            modeType.classList.add('unclickable');
            
            checkbox.checked = false;
        })
    })
});