document.addEventListener("DOMContentLoaded", function () {
    const mainImage = document.querySelector(".main-image");
    const imageContainer = document.querySelector(".image-container");
    const messageContainer = document.getElementById('message-container');

    imageContainer.addEventListener("click", function (e) {
        const clickedImage = e.target.closest(".image:not(.main-image)");

        if (!clickedImage) return;

        const updateImageUrl = this.getAttribute("data-update-image-url");

        if (!updateImageUrl) {
            console.error('Error: data-update-image-url is missing or invalid.');
            return;
        }

        const productId = parseInt(clickedImage.getAttribute("data-product-id"), 10);
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
            },
            body: JSON.stringify({ clickedImageId, productId })
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
                showMessage("Main Image successfully updated.", "success");
                console.log('Success:', data);
            })
            .catch(error => {
                showMessage(error.message, "error");
                console.error('Error:', error);
            });
    });

    // Adding event listeners for mouseover and mouseout on the imageContainer
    imageContainer.addEventListener("mouseover", handleMouseOver);
    imageContainer.addEventListener("mouseout", handleMouseOut);

    function handleMouseOver(e) {
        const hoveredImage = e.target.closest(".image:not(.main-image)");
        const hoveredBtn = e.target.closest(".btn-danger");

        if (!hoveredImage && !hoveredBtn) return;

        const hoveredImageDeleteBtn = hoveredBtn ?? hoveredImage.previousElementSibling;
        if (!hoveredImageDeleteBtn) return;

        const removeImageUrl = imageContainer.getAttribute("data-remove-image-url");
        hoveredImageDeleteBtn.classList.remove("invisible");

        hoveredImageDeleteBtn.addEventListener("click", handleDeleteClick);

        function handleDeleteClick() {
            const imageId = hoveredImage.getAttribute("data-id");

            fetch(`${removeImageUrl}/${imageId}`, { method: 'DELETE' })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok.');
                    }
                    return response.json();
                })
                .then(data => {
                    renderUi(hoveredImage);
                    showMessage("Image successfully removed.", "success");
                    console.log('Success:', data);
                })
                .catch(error => {
                    showMessage(error.message, "error");
                    console.error('Error:', error);
                });
        }
    }

    function handleMouseOut(e) {
        const hoveredImage = e.target.closest(".image:not(.main-image)");
        if (!hoveredImage) return;

        const hoveredImageDeleteBtn = hoveredImage.previousElementSibling;
        if (!hoveredImageDeleteBtn) return;

        hoveredImageDeleteBtn.classList.add("invisible");
    }

    function renderUi(imageElement) {
        // Remove the image element
        imageElement.parentElement.remove();

        // Check if there are remaining images
        const remainingImages = imageContainer.querySelectorAll(".image").length;

        // Update the UI to reflect the number of remaining images
        if (remainingImages === 0) {
            const imageContainer = document.querySelector(".image-container");
            const noImageText = document.createElement("p");
            noImageText.textContent = "There is no image in this product!";
            imageContainer.parentElement.appendChild(noImageText);
            imageContainer.remove();
        }
    }

    function showMessage(message, type) {
        // Options for the toast
        var options = {
            text: message,
            duration: 2500,
            callback: function () {
                console.log("Toast hidden");
                Toastify.reposition();
            },
            close: true,
            style: {}
        };

        // Configure the style based on the message type
        if (type === "success") {
            options.style.background = "green";
        } else if (type === "error") {
            options.style.background = "red";
        }

        // Initializing the toast
        var myToast = Toastify(options);

        // Toast
        myToast.showToast();
    }
});
