mergeInto(LibraryManager.library,
          {
            SendMessageToPlane: function () {
              SendMessage('Plane', 'PlaneInitCallback');
            },

            Hello: function () {
              window.alert("  +++++++++++++Hello, world!");
            },
          });
