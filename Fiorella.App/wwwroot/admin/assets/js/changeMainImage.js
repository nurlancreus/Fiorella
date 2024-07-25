document.addEventListener("DOMContentLoaded", function () {
    const mainImage = document.querySelector(".main-image");
    const imageContainer = document.querySelector(".image-container");

    //const verificationTokenElement = document.querySelector('input[name="__RequestVerificationToken"]');

    //if (!verificationTokenElement) {
    //    console.error('Error: CSRF token input element not found.');
    //    return;
    //}

    // const verificationToken = verificationTokenElement.value;

    imageContainer.addEventListener("click", function (e) {
        const clickedImage = e.target.closest(".image:not(.main-image)");

        if (!clickedImage) return;

        const updateImageUrl = this.getAttribute("data-update-image-url");

        if (!updateImageUrl) {
            console.error('Error: data-update-image-url is missing or invalid.');
            return;
        }

        const clickedImageId = parseInt(clickedImage.getAttribute("data-id"), 10);

        function swapImages() {
            const clickedImageSrc = clickedImage.getAttribute("src");
            const oldMainImageSrc = mainImage.getAttribute("src");

            mainImage.setAttribute("src", clickedImageSrc);
            clickedImage.setAttribute("src", oldMainImageSrc);
        }

        fetch(updateImageUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                //'X-CSRF-TOKEN': verificationToken
            },
            body: JSON.stringify({ mainImageId: clickedImageId })
        })
            .then(response => {
                console.log(response);
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
                return response.json();
            })
            .then(data => {
                swapImages();
                console.log('Success:', data);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });

    imageContainer.addEventListener("mouseenter", function (e) {
        const hoveredImage = e.target.closest(".image:not(.main-image)");

        if (!hoveredImage) return;
        const hoveredImageDeleteBtn = hoveredImage.previousElementSibling;

        if (!hoveredImageDeleteBtn) return;

        const removeImageUrl = this.getAttribute("data-remove-image-url");
        hoveredImageDeleteBtn.addEventListener("click", function (e) {
            const imageId = this.nextElementSibling.getAttribute("data-id");

            fetch(`${removeImageUrl}/${imageId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then(response => {
                    console.log(response);
                    if (!response.ok) {
                        throw new Error('Network response was not ok.');
                    }
                    return response.json();
                })
                .then(data => {
                    swapImages();
                    console.log('Success:', data);
                })
                .catch(error => {
                    console.error('Error:', error);
                });
        })
    })
});
