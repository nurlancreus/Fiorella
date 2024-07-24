document.addEventListener("DOMContentLoaded", function () {
    const images = document.querySelectorAll(".image");
    const imageContainer = document.querySelector(".image-container");

    imageContainer.addEventListener("click", function (e) {
        // Class border-danger added to the main image
        const clickedImage = e.target.closest(".image:not(.border-danger)")
        console.log(clickedImage);

        // if we clicked main image again or do not clicked ny image at all, we do not want to trigger any operations
        if (!clickedImage) return;
        console.log(clickedImage);
        const clickedImageId = clickedImage.getAttribute("data-id");

        // Remove the 'border-danger' class from all images
        images.forEach(c => c.classList.remove("border-danger"));

        // Add the 'border-danger' class to the clicked image
        clickedImage.classList.add("border-danger");

        // Optionally, you can add an AJAX call here to update the main image on the server side
        // Make an AJAX call to update the main image on the server
        fetch('/Product/UpdateMainImage', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': document.querySelector('input[name="__RequestVerificationToken"]').value // For CSRF protection
            },
            body: JSON.stringify({ mainImageId: clickedImageId })
        })
            .then(response => response.json())
            .then(data => {
                // Handle the response from the server if needed
                console.log('Success:', data);
            })
            .catch(error => {
                console.error('Error:', error);
            });
    });
});