mergeInto(LibraryManager.library,
          {
            ShowAd: function() {
              ysdk.adv.showRewardedVideo({
                callbacks: {
                  onOpen: () => {
                    console.log('Video ad open.');
                  },
                  onRewarded: () => {
                    myGameInstance.SendMessage('Main Menu', 'Repair')
                  },
                  onClose: () => {
                    console.log('Video ad closed.');
                  },
                  onError: (e) => {
                    console.log('Error while open video ad:', e);
                  }
                }
              })
            },

            Hello: function () {
              window.alert("  +++++++++++++Hello, world!");
            },
          });
