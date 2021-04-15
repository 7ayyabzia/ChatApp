$(function () {
    $userId = $('input[name=UserID]').val();
    $recipient = { type: 'Contact', id: 0, name: '' };
    $chatEl = $('#chat-conversation ul');
    $chatWith = $('#chat .chat-with');
    $spinner = {
        start: () => {
            $chatWith.append(`<div class="spinner-border" style="position: relative; top:0; margin: 0 10px; width: 20px; height: 20px"></div>`);
        },
        end: () => {
            let PL = $chatWith.find('.spinner-border');
            PL.remove();
        }
    }

    $('.nav-tabs a').click(function () {
        $('.nav-tabs div').removeClass('active');
        let el = $(this).attr('href');
        $('.people-list .tab-content').hide();
        $(el).show();
        setTimeout(() => { $(this).parent().addClass('active') }, 100);

        if (el == '#groups' && $('.groups li').length > 0) $('.groups li')[0].click();
        if (el == '#users' && $('.contacts li').length > 0) $('.contacts li')[0].click();
    })
    $('.contacts li').click(function () {
        $spinner.start();
        $el = $(this)
        $('.contacts li').removeClass('active');
        $el.addClass('active');

        $recipient = { type: 'Individual', id: $el.data('id'), name: $el.data('name') };
        JsonCallParam("Chat", "GetUserMessages", { recipientId: $recipient.id }).then(() => {
            updateDOM(JSON.parse(list));
        });
    })
    $('.groups li').click(function () {
        $spinner.start();
        $el = $(this)
        $('.groups li').removeClass('active');
        $el.addClass('active');

        $recipient = { type: 'Group', id: $el.data('id'), name: $el.data('name') };
        JsonCallParam("Chat", "GetGroupMessages", { id: $recipient.id }).then(() => {
            updateDOM(JSON.parse(list));
        });
    })
    function updateDOM(list) {
        $('.chat-with .name').html($recipient.name);
        $chatEl.empty();
        list.forEach((obj) => {
            let messageFrom = $userId == obj.UserID ? 'my-message' : 'friend-message';
            $chatEl.append(`<li class="clearfix ${messageFrom}">
                <div class="message-data">
                    <span class="message-data-time">
                        ${new Date(obj.TimeStamp)} <strong>${obj.User.Name}</strong>
                    </span>
                </div>
                <div class="message">
                    <p class="m-b-0">
                        <span class="sms-body">${obj.MessageText}</span>
                    </p>
                </div>
            </li>`);
        })
        setTimeout(() => { $('#chat-conversation').animate({ scrollTop: 9999 }, 500); $spinner.end() }, 200);
    }

    $('#message-box').keypress(function (e) {
        if (e.which == 13) {
            e.stopPropagation();
            e.preventDefault();
            sendMessage()
        }
    });

    function sendMessage() {
        $spinner.start();
        let message = {
            MessageText: $('#message-box').val(),
            RecipientType: $recipient.type,
            RecipientID: $recipient.id.toString(),
            UserID: $userId
        };
        JsonCallParam("Chat", "SendMessage", { "message": JSON.stringify(message) })
            .then(() => {
                if (list.StatusCode == 200) {
                    message = JSON.parse(list.Response);
                    $chatEl.append(`<li class="clearfix my-message">
                        <div class="message-data">
                            <span class="message-data-time">
                                ${new Date(message.TimeStamp)} <strong>${message.User.Name}</strong>
                            </span>
                        </div>
                        <div class="message">
                            <p class="m-b-0">
                                <span class="sms-body">${message.MessageText}</span>
                            </p>
                        </div>
                    </li>`);
                    $('#message-box').val('');
                    setTimeout(() => { $('#chat-conversation').animate({ scrollTop: 9999 }, 500); $spinner.end() }, 200);
                }
            })
        
    }

    chat.client.receiveMessage = function (message) {
        message = JSON.parse(message);

        if (message.RecipientID != $recipient.id && message.RecipientType == "Group") return;
        if (message.UserID != $recipient.id && message.RecipientType == "Individual") return;

        $chatEl.append(`<li class="clearfix friend-message">
                <div class="message-data">
                    <span class="message-data-time">
                         ${new Date(message.TimeStamp)} <strong>${message.User.Name}</strong>
                    </span>
                </div>
                <div class="message">
                    <p class="m-b-0">
                        <span class="sms-body">${message.MessageText}</span>
                    </p>
                </div>
            </li>`);
        setTimeout(() => { $('#chat-conversation').animate({ scrollTop: 9999 }, 500) }, 200);
    };


    $('.contacts li')[0].click();
});